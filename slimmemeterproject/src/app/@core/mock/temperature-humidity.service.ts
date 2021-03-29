import { Injectable } from '@angular/core';
import { of as observableOf,  Observable } from 'rxjs';
import { TemperatureHumidityData } from '../data/temperature-humidity';
import { IwsnBackendService } from '../../iwsn-backend/iwsn-backend.service';
import { Datagram } from '../../models/Datagram';

@Injectable()
export class TemperatureHumidityService extends TemperatureHumidityData {
  datagram: Datagram[] = [];

  constructor(public iwsnBackendService: IwsnBackendService) {
    super();
  }

  getTemperatureData(): Observable<number> {
    // this.iwsnBackendService.getMeasurements().subscribe(item => this.datagram = item);
    //return observableOf(this.datagram[0].Telegram.InstantaneousElectricityUsage);
    return observableOf(100);
  }
}