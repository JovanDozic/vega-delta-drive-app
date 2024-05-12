import { Component } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { VehicleSearchRequest } from '../model/vehicle-search-request.model';
import { Vehicle } from '../model/vehicle.model';
import { VehicleSearchResponse } from '../model/vehicle-search-response.model';
import { VehicleBookingRequest } from '../model/vehicle-booking-request.model';
import { TokenService } from '../services/authentication/token.service';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { VehicleBookingService } from '../services/vehicle-booking.service';

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
    private authService: AuthenticationService
  ) {}

  findVehicles() {
    this.availableVehicles = [];

    this.vehicleService
      .getAvailableVehicles(this.search)
      .subscribe((response) => {
        this.availableVehicles = response.results;
      });
  }

  sendRequest(vehicle: VehicleSearchResponse) {
    const request: VehicleBookingRequest = {
      id: -1,
      userId: this.authService.getUserId() ?? -1,
      vehicleId: vehicle.id,
      startLocation: this.search.startLocation,
      endLocation: this.search.endLocation,
    };

    this.bookingService.sendBookingRequest(request).subscribe((response) => {
      console.log('Booking request sent', response);
      // TODO: Redirect user to booking tracking page for created booking.
    });
  }
}
