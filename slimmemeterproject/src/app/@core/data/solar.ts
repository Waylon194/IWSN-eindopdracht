import { Observable } from 'rxjs';
import { IwsnBackendService } from '../../iwsn-backend/iwsn-backend.service';

export abstract class SolarData {
  constructor(public iwsnBackendService: IwsnBackendService) {}

  abstract getSolarData(): Observable<number>;
}
