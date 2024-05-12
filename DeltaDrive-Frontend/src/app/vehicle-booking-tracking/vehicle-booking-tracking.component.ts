import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import { VehicleBooking } from '../model/vehicle-booking.model';
import { Vehicle } from '../model/vehicle.model';
import { User } from '../model/user.model';
import { AuthenticationService } from '../services/authentication/authentication.service';

@Component({
  selector: 'app-vehicle-booking-tracking',
  templateUrl: './vehicle-booking-tracking.component.html',
  styleUrl: './vehicle-booking-tracking.component.css',
})
export class VehicleBookingTrackingComponent implements OnInit {
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
    private authService: AuthenticationService
  ) {}

  ngOnInit() {
    // TODO: Handle error when user tries to access the page without an ID
    // TODO: Handle error when user tries to view someone else's booking

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
}
