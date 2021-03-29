import { Injectable } from '@angular/core';
import { of as observableOf, Observable } from 'rxjs';
import { ElectricityChart, ElectricityData } from '../data/electricity';
import { IwsnBackendService } from '../../iwsn-backend/iwsn-backend.service';

@Injectable()
export class ElectricityService extends ElectricityData {

  private chartPoints = [

  ];

  chartData: ElectricityChart[];

  constructor(public iwsnBackendService: IwsnBackendService) {
    super(iwsnBackendService);   

    this.chartData = this.chartPoints.map((p, index) => ({
      //Creating of labels
      label: (index % 10 === 0) ? `${Math.round(index / 10)}` : '',
      value: p,
    }));
  }

  getChartData(): Observable<ElectricityChart[]> {
    return observableOf(this.chartData);
  }
}