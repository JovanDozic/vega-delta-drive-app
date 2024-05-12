import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';

@Component({
  selector: 'app-vehicle-booking-tracking',
  templateUrl: './vehicle-booking-tracking.component.html',
  styleUrl: './vehicle-booking-tracking.component.css',
})
export class VehicleBookingTrackingComponent implements OnInit {
  id: number = -1;

  constructor(
    private route: ActivatedRoute,
    private bookingService: VehicleBookingService
  ) {}

  ngOnInit() {
    // TODO: check if requested booking is users booking

    this.route.paramMap.subscribe((params) => {
      const idParam = params.get('id');
      if (idParam !== null) {
        this.id = +idParam;

        this.bookingService.getBooking(this.id).subscribe((booking) => {
          console.log('Booking details');
          console.log(booking);
        });
      }
    });
  }
}
