import { Component } from '@angular/core';
import { Location } from '../model/location.model';

@Component({
  selector: 'app-vehicle-booking',
  templateUrl: './vehicle-booking.component.html',
  styleUrl: './vehicle-booking.component.css',
})
export class VehicleBookingComponent {
  currentLocation: Location = {};
  destinationLocation: Location = {};

  constructor() {}
}
