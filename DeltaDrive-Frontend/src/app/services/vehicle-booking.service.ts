import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VehicleBookingRequest } from '../model/vehicle-booking-request.model';
import { environment } from '../../environments/environment';
import { VehicleBooking } from '../model/vehicle-booking.model';
import { VehicleBookingResponse } from '../model/vehicle-booking-response.model';
import { Result } from '../model/common/result.model';

@Injectable({
  providedIn: 'root',
})
export class VehicleBookingService {
  constructor(private http: HttpClient) {}

  sendBookingRequest(request: VehicleBookingRequest) {
    return this.http.post<Result<VehicleBookingResponse>>(
      `${environment.apiHost}/VehicleBooking/sendRequest`,
      request
    );
  }

  getBooking(id: number) {
    return this.http.get<Result<VehicleBooking>>(
      `${environment.apiHost}/VehicleBooking/getBooking/${id}`
    );
  }

  startRide(id: number) {
    return this.http.get<any>(
      `${environment.apiHost}/VehicleBooking/startRide/${id}`,
      {}
    );
  }
}
