import { Injectable } from '@angular/core';
import { of as observableOf,  Observable } from 'rxjs';
import { SolarData } from '../data/solar';
import { IwsnBackendService } from '../../iwsn-backend/iwsn-backend.service';

@Injectable()
export class SolarService extends SolarData {
  private value = 0;

  constructor(public iwsnBackendService: IwsnBackendService) {
    super(iwsnBackendService);
  }

  getSolarData(): Observable<number> {
    return observableOf(this.value);
  }
}
