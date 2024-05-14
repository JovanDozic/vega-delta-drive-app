import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VehicleSearchRequest } from '../model/vehicle-search-request.model';
import { environment } from '../../environments/environment';
import { PagedResult } from '../model/common/paged-result.model';
import { Vehicle } from '../model/vehicle.model';
import { VehicleSearchResponse } from '../model/vehicle-search-response.model';
import { catchError, map, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(private http: HttpClient) {}

  getAvailableVehicles(search: VehicleSearchRequest) {
    return this.http
      .post<PagedResult<VehicleSearchResponse>>(
        `${environment.apiHost}/Vehicle/getAvailable`,
        search
      )
      .pipe(
        map((response) => response),
        catchError((error) => {
          console.error('Error fetching available vehicles:', error);
          return throwError(() => {
            error;
          });
        })
      );
  }

  updateVehicleLocation(vehicle: Vehicle) {
    return this.http
      .patch<void>(`${environment.apiHost}/Vehicle/updateLocation`, vehicle)
      .pipe(
        map((response) => response),
        catchError((error) => {
          console.error('Error updating vehicle location:', error);
          return throwError(() => {
            error;
          });
        })
      );
  }
}
