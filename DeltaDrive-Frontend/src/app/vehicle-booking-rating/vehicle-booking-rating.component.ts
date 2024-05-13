import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import {
  VehicleBooking,
  VehicleBookingStatus,
} from '../model/vehicle-booking.model';
import { VehicleBookingRating } from '../model/vehicle-booking-rating.model';

@Component({
  selector: 'app-vehicle-booking-rating',
  templateUrl: './vehicle-booking-rating.component.html',
  styleUrl: './vehicle-booking-rating.component.css',
})
export class VehicleBookingRatingComponent implements OnInit {
  id: number = -1;
  doesNotExist: boolean = false;
  isAlreadyRated: boolean = false;
  booking: VehicleBooking = {} as VehicleBooking;
  bookingRating: VehicleBookingRating = {} as VehicleBookingRating;

  constructor(
    private route: ActivatedRoute,
    private bookingService: VehicleBookingService
  ) {}

  ngOnInit(): void {
    this.fetchBookingDetails();
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
              this.isAlreadyRated = this.booking.rating !== null;
              if (this.isAlreadyRated) {
                this.bookingRating = this.booking
                  .rating as VehicleBookingRating;
              }
            }
          },
          (error) => {
            this.doesNotExist = true;
          }
        );
      }
    });
  }

  saveRating() {
    // TODO: Add validation
    this.booking.rating = this.bookingRating;
    this.bookingService.updateBooking(this.booking).subscribe(
      (response) => {
        console.log('Rating saved', response);
        this.isAlreadyRated = true;
      },
      (error) => {
        console.error('Error saving rating', error);
      }
    );
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
        return 'Unknown Status'; // In case there's an undefined status
    }
  }
}
