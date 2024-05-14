import { Component, OnInit } from '@angular/core';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import {
  VehicleBooking,
  VehicleBookingStatus,
} from '../model/vehicle-booking.model';
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

  getStatusAsString(status: VehicleBookingStatus): string {
    switch (status) {
      case VehicleBookingStatus.Waiting:
        return 'Waiting';
      case VehicleBookingStatus.DrivingToStartLocation:
        return 'Driving to Start Location';
      case VehicleBookingStatus.WaitingForPassenger:
        return 'Waiting for Passenger';
      case VehicleBookingStatus.DrivingToEndLocation:
        return 'Driving to End Location';
      case VehicleBookingStatus.Completed:
        return 'Completed';
      default:
        return 'Unknown Status';
    }
  }
}
