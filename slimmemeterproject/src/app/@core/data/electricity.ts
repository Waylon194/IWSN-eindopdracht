import { Observable } from 'rxjs';
import { IwsnBackendService } from '../../iwsn-backend/iwsn-backend.service';

export interface Month {
  month: string;
  delta: string;
  down: boolean;
  kWatts: string;
  cost: string;
}

export interface Electricity {
  title: string;
  active?: boolean;
  months: Month[];
}

export interface ElectricityChart {
  label: string;
  value: number;
}

export abstract class ElectricityData {
  constructor(public iwsnBackendService: IwsnBackendService) {}
  abstract getChartData(): Observable<ElectricityChart[]>;
}
