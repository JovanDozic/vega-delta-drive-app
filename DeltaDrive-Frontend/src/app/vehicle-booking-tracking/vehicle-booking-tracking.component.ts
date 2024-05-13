import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import {
  VehicleBooking,
  VehicleBookingStatus,
} from '../model/vehicle-booking.model';
import { Vehicle } from '../model/vehicle.model';
import { User } from '../model/user.model';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { SignalRService } from '../services/signal-r.service';

@Component({
  selector: 'app-vehicle-booking-tracking',
  templateUrl: './vehicle-booking-tracking.component.html',
  styleUrl: './vehicle-booking-tracking.component.css',
})
export class VehicleBookingTrackingComponent implements OnInit, OnDestroy {
  id: number = -1;
  booking: VehicleBooking = {
    startLocation: {},
    endLocation: {},
    vehicle: { location: { x: 0, y: 0 } },
  } as VehicleBooking;
  vehicle: Vehicle = { location: { x: 0, y: 0 } } as Vehicle;
  passenger: User = {};
  statusMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private bookingService: VehicleBookingService,
    private authService: AuthenticationService,
    private signalRService: SignalRService
  ) {}

  ngOnInit() {
    // TODO: Handle error when user tries to access the page without an ID
    // TODO: Handle error when user tries to view someone else's booking

    this.fetchBookingDetails();
    this.initSignalR();
  }

  ngOnDestroy() {
    this.signalRService.stopConnection();
  }

  initSignalR() {
    this.signalRService.startConnection();
    this.signalRService.addLocationListener((id, status, lat, long) => {
      console.log('Received location', id, status, lat, long);

      if (this.booking.id != id) {
        throw new Error('Invalid booking ID');
      }

      this.booking.vehicle.location.x = long;
      this.booking.vehicle.location.y = lat;
      this.booking.status = status;
      this.updateStatus(status);
    });
  }

  fetchBookingDetails() {
    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');
      if (idParam !== null) {
        this.id = +idParam;

        this.bookingService.getBooking(this.id).subscribe((response) => {
          console.log('Response details', response);

          if (response.value) {
            this.booking = response.value;
            this.vehicle = response.value.vehicle;
          }
        });
      }
    });
  }

  startRideToStartLocation() {
    this.bookingService
      .startRideToStartLocation(this.booking.id)
      .subscribe((response) => {
        console.log('Ride started', response);
      });
  }

  startRideToEndLocation() {
    this.bookingService
      .startRideToEndLocation(this.booking.id)
      .subscribe((response) => {
        console.log('Ride started', response);
      });
  }

  private updateStatus(status: number) {
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
