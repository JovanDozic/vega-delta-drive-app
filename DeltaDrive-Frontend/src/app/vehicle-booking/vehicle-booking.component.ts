import { Component } from '@angular/core';
import { VehicleService } from '../services/vehicle.service';
import { VehicleSearchRequest } from '../model/vehicle-search-request.model';
import { Vehicle } from '../model/vehicle.model';
import { VehicleSearchResponse } from '../model/vehicle-search-response.model';

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

  constructor(private vehicleService: VehicleService) {}

  findVehicles() {
    this.availableVehicles = [];

    this.vehicleService
      .getAvailableVehicles(this.search)
      .subscribe((response) => {
        this.availableVehicles = response.results;
        console.log('EVO GA:');
        console.log(this.availableVehicles);
        console.log('IMA IH: ', this.availableVehicles.length);
      });
  }
}
