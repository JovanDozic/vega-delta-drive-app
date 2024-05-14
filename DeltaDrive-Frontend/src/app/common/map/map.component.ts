import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import mapboxgl from 'mapbox-gl';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrl: './map.component.css',
})
export class MapComponent implements AfterViewInit, OnChanges {
  @Input() lng: number = 0;
  @Input() lat: number = 0;
  @Input() clickable: boolean = false;
  @Input() containerId: string = 'map';
  @Output() coordinatesChanged = new EventEmitter<{
    lng: number;
    lat: number;
  }>();
  map!: mapboxgl.Map;
  marker!: mapboxgl.Marker;

  ngAfterViewInit() {
    try {
      (mapboxgl as any).accessToken = environment.mapbox.accessToken;
      this.map = new mapboxgl.Map({
        container: this.containerId,
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [this.lng, this.lat],
        zoom: 12,
      });

      this.marker = new mapboxgl.Marker()
        .setLngLat([this.lng, this.lat])
        .addTo(this.map);

      if (this.clickable) {
        this.map.on('click', (event) => {
          const { lng, lat } = event.lngLat;
          this.marker.setLngLat([lng, lat]);
          this.lng = lng;
          this.lat = lat;
          this.coordinatesChanged.emit({ lng, lat });
        });
      }
    } catch (error) {
      console.error('Error initializing the map:', error);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    try {
      if (changes['lng'] || changes['lat']) {
        if (this.marker) {
          this.marker.setLngLat([this.lng, this.lat]);
          if (!this.clickable) this.map.setCenter([this.lng, this.lat]);
        }
      }
    } catch (error) {
      console.error('Error updating the marker position:', error);
    }
  }
}
