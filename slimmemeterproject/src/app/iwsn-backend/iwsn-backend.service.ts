import { Injectable } from '@angular/core';
import { HttpClient, } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { SmartMeterMeasurement } from '../models/MongoDBSmartMeterModel';
import { TemperatureMeasurement } from '../models/MongoDBTemperatureModel';

@Injectable({
  providedIn: 'root'
})
export class IwsnBackendService {
  constructor(private http: HttpClient) { }
  // latest/electric/all/async

  getLatestTemp() : Observable<TemperatureMeasurement>{
    //Single
    return this.http.get<SmartMeterMeasurement>("http://localhost:5000/backend-api/v1/iwsn-temperature/latest/async")
    .pipe(map((res: any) => res))
  }

  getLatestMeasurement() : Observable<SmartMeterMeasurement>{
    //Single
    return this.http.get<SmartMeterMeasurement>("http://localhost:5000/backend-api/v1/iwsn-smartmeter/latest/single/async")
    .pipe(map((res: any) => res))
  }

  getLatestElectricityMeasurements() : Observable<number[]>{
    //All electricity
    return this.http.get<number[]>("http://localhost:5000/backend-api/v1/iwsn-smartmeter/latest/electric/all/async")
    .pipe(map((res: any) => res))
  }
}