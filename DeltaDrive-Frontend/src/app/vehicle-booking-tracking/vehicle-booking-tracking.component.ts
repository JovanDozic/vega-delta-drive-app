import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import {
  VehicleBooking,
  VehicleBookingStatus,
} from '../model/vehicle-booking.model';
import { Vehicle } from '../model/vehicle.model';
import { User } from '../model/user.model';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { SignalRService } from '../services/signal-r.service';
import { VehicleService } from '../services/vehicle.service';

@Component({
  selector: 'app-vehicle-booking-tracking',
  templateUrl: './vehicle-booking-tracking.component.html',
  styleUrl: './vehicle-booking-tracking.component.css',
})
export class VehicleBookingTrackingComponent implements OnInit, OnDestroy {
  id: number = -1;
  booking: VehicleBooking = {
    startLocation: { latitude: 0, longitude: 0 },
    endLocation: { latitude: 0, longitude: 0 },
    vehicle: { location: { x: 0, y: 0 } },
  } as VehicleBooking;
  isCompleted: boolean = false;
  doesNotExist: boolean = false;
  vehicle: Vehicle = { location: { x: 0, y: 0 } } as Vehicle;
  passenger: User = {};
  statusMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookingService: VehicleBookingService,
    private signalRService: SignalRService,
    private vehicleService: VehicleService
  ) {}

  ngOnInit() {
    this.fetchBookingDetails();
  }

  ngOnDestroy() {
    this.signalRService.stopConnection();
  }

  initSignalR() {
    if (this.doesNotExist) return;
    this.signalRService.startConnection();
    this.signalRService.addLocationListener(
      (id, status, long, lat, currentPrice) => {
        console.log('Received location', id, status, long, lat, currentPrice);
        if (this.booking.id != id) {
          throw new Error('Invalid booking ID');
        }

        this.booking.price = currentPrice;
        this.booking.vehicle.location.x = long;
        this.booking.vehicle.location.y = lat;
        this.booking.status = status;
        this.updateStatusMessage(status);

        // As a workaround for the issue with DI on the Backend, we will update the location of the vehicle trough a request from the Frontend - when status changes accordingly.
        // This is not the best solution, but for simulation purposes it will work as proof of concept - as our app does not have second role (driver).

        if (status == VehicleBookingStatus.WaitingForPassenger) {
          this.vehicleService
            .updateVehicleLocation(this.booking.vehicle)
            .subscribe((response) => {
              console.log(
                'Location updated after driving to start location.',
                response
              );
            });
        }

        if (status == VehicleBookingStatus.Completed) {
          this.vehicleService
            .updateVehicleLocation(this.booking.vehicle)
            .subscribe((response) => {
              console.log(
                'Location updated after driving to end location.',
                response
              );
            });

          this.booking.status = VehicleBookingStatus.Completed;
          this.bookingService
            .completeBooking(this.booking)
            .subscribe((response) => {
              console.log('Booking completed', response);
            });
        }
      }
    );
  }

  fetchBookingDetails() {
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');
      if (idParam !== null) {
        this.id = +idParam;

        this.bookingService.getBooking(this.id).subscribe(
          (response) => {
            console.log('Response details', response);

            if (response.value) {
              this.booking = response.value;
              this.vehicle = response.value.vehicle;
              this.isCompleted = response.value.status == 4;

              this.initSignalR();
            }
          },
          (error) => {
            this.doesNotExist = true;
          }
        );
      }
    });
  }

  startRideToStartLocation() {
    this.bookingService
      .startRideToStartLocation(this.booking.id)
      .subscribe((response) => {
        console.log('Ride to start location started', response);
      });
  }

  startRideToEndLocation() {
    this.bookingService
      .startRideToEndLocation(this.booking.id)
      .subscribe((response) => {
        console.log('Ride to end location started', response);
      });
  }

  rateRide() {
    this.router.navigate(['/vehicle-booking-rating/' + this.booking.id]);
  }

  onVehicleLocationChanged(event: { lng: number; lat: number }) {
    this.booking.vehicle.location.y = event.lat;
    this.booking.vehicle.location.x = event.lng;
  }

  onStartLocationChanged(event: { lng: number; lat: number }) {
    this.booking.startLocation.latitude = event.lat;
    this.booking.startLocation.longitude = event.lng;
  }

  onEndLocationChanged(event: { lng: number; lat: number }) {
    this.booking.startLocation.latitude = event.lat;
    this.booking.startLocation.longitude = event.lng;
  }

  private updateStatusMessage(status: number) {
    switch (status) {
      case 0:
        this.statusMessage = 'Waiting for start';
        return;
      case 1:
        this.statusMessage = 'Driving to start location';
        return;
      case 2:
        this.statusMessage = 'Waiting for passenger';
        return;
      case 3:
        this.statusMessage = 'Driving to end location';
        return;
      case 4:
        this.statusMessage = 'Completed';
        return;
      default:
        this.statusMessage = 'Unknown';
        return;
    }
  }
}
