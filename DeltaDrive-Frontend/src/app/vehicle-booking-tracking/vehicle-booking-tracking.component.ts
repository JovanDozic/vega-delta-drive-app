import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import { VehicleBooking } from '../model/vehicle-booking.model';
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
    this.signalRService.addLocationListener((id, lat, long) => {
      console.log('Received location', id, lat, long);
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

  startRide() {
    this.bookingService.startRide(this.booking.id).subscribe((response) => {
      console.log('Ride started', response);
    });
  }
}
