import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VehicleBookingRequest } from '../model/vehicle-booking-request.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VehicleBookingService {
  constructor(private http: HttpClient) {}

  sendBookingRequest(request: VehicleBookingRequest) {
    return this.http.post(
      `${environment.apiHost}/VehicleBooking/sendRequest`,
      request
    );
  }
}
