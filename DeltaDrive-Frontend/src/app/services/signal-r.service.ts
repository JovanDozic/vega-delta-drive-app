import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | undefined;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalRHost + '/vehicleLocationHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  public addLocationListener = (
    updateLocation: (id: number, lat: number, long: number) => void
  ) => {
    this.hubConnection?.on(
      'ReceiveLocation',
      (bookingId, latitude, longitude) => {
        updateLocation(bookingId, latitude, longitude);
      }
    );
  };

  public stopConnection = () => {
    this.hubConnection?.stop();
  };
}
