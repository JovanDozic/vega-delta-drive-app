import { Component } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { VehicleSearchRequest } from '../model/vehicle-search-request.model';
import { Vehicle } from '../model/vehicle.model';
import { VehicleSearchResponse } from '../model/vehicle-search-response.model';
import { VehicleBookingRequest } from '../model/vehicle-booking-request.model';
import { TokenService } from '../services/authentication/token.service';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { VehicleBookingService } from '../services/vehicle-booking.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicle-booking',
  templateUrl: './vehicle-booking.component.html',
  styleUrl: './vehicle-booking.component.css',
})
export class VehicleBookingComponent {
  search: VehicleSearchRequest = {
    startLocation: {
      latitude: 0,
      longitude: 0,
    },
    endLocation: {
      latitude: 0,
      longitude: 0,
    },
  };
  availableVehicles: VehicleSearchResponse[] = [];
  // TODO: Add loading boolean

  constructor(
    private vehicleService: VehicleService,
    private bookingService: VehicleBookingService,
    private authService: AuthenticationService,
    private router: Router
  ) {}

  findVehicles() {
    this.availableVehicles = [];

    this.vehicleService
      .getAvailableVehicles(this.search)
      .subscribe((response) => {
        this.availableVehicles = response.results;
        console.log('Available vehicles', this.availableVehicles);
      });
  }

  sendRequest(vehicle: VehicleSearchResponse) {
    // TODO: Handle case where user already has a booking (overall: handle all error Result objects correctly)

    const request: VehicleBookingRequest = {
      id: -1,
      userId: this.authService.getUserId() ?? -1,
      vehicleId: vehicle.id,
      startLocation: this.search.startLocation,
      endLocation: this.search.endLocation,
    };

    vehicle.isLoadingRequest = true;

    this.bookingService.sendBookingRequest(request).subscribe((response) => {
      console.log('Booking request sent', response);
      if (response.value != null && response.value.isAccepted) {
        document.getElementById('cancelButton')?.click();

        this.router.navigate([
          '/vehicle-booking-tracking/' + response.value.bookingId,
        ]);
      } else if (response.value != null && !response.value.isAccepted) {
        vehicle.isLoadingRequest = false;
        alert(
          'Booking request not accepted. Please consider any other vehicle.'
        );
        vehicle.didDecline = true;
      } else {
        alert('Booking request failed. Please try again.');
      }
    });
  }
}
