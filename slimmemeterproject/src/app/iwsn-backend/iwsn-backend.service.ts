import { Injectable } from '@angular/core';
import { HttpClient, } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { SmartMeterMeasurement } from '../models/MongoDB';

@Injectable({
  providedIn: 'root'
})
export class IwsnBackendService {
  constructor(private http: HttpClient) { }

  // latest/electric/all/async

  getLatestMeasurement() : Observable<SmartMeterMeasurement>{
    //Single
    return this.http.get<SmartMeterMeasurement>("http://localhost:5000/backend-api/v1/iwsn/latest/single/async")
    .pipe(map((res: any) => res))
  }

  getLatestElectricityMeasurements() : Observable<number[]>{
    //All electricity
    return this.http.get<number[]>("http://localhost:5000/backend-api/v1/iwsn/latest/electric/all/async")
    .pipe(map((res: any) => res))
  }
}