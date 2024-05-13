import { Component, OnInit } from '@angular/core';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import { VehicleBooking } from '../model/vehicle-booking.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-booking-history',
  templateUrl: './vehicle-booking-history.component.html',
  styleUrl: './vehicle-booking-history.component.css',
})
export class VehicleBookingHistoryComponent implements OnInit {
  bookings: VehicleBooking[] = [];

  constructor(
    private bookingService: VehicleBookingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchBookingHistory();
  }

  fetchBookingHistory() {
    this.bookingService.getHistory().subscribe((bookings) => {
      this.bookings = bookings;
      console.log(this.bookings);
    });
  }

  openBookingTracking(booking: VehicleBooking) {
    this.router.navigate(['vehicle-booking-tracking/' + booking.id]);
  }
}
