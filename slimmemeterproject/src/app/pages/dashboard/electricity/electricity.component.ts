import { Component, OnDestroy, OnInit } from '@angular/core';
import { NbThemeService } from '@nebular/theme';

import { Electricity, ElectricityChart, ElectricityData } from '../../../@core/data/electricity';
import { takeWhile } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { IwsnBackendService } from '../../../iwsn-backend/iwsn-backend.service'; 
import { Datagram } from '../../../models/Datagram';

@Component({
  selector: 'ngx-electricity',
  styleUrls: ['./electricity.component.scss'],
  templateUrl: './electricity.component.html',
})
export class ElectricityComponent implements OnDestroy, OnInit {
  private alive = true;

  listData: Electricity[];
  data: ElectricityChart[] = [];
  chartData: ElectricityChart[];
  currentTheme: string;
  themeSubscription: any;
  measurements: Datagram[] = [];
  public totalPowerConsumption: number = 0;

  constructor(private electricityService: ElectricityData, private themeService: NbThemeService, private backend_service: IwsnBackendService) {
    this.themeService.getJsTheme()
      .pipe(takeWhile(() => this.alive))
      .subscribe(theme => {
        this.currentTheme = theme.name;
    });

    forkJoin(
      this.electricityService.getChartData(),
    )
      .pipe(takeWhile(() => this.alive))
      .subscribe(([chartData]: [ElectricityChart[]] ) => {
        this.chartData = chartData;
      });
  }
  ngOnInit(): void {
    this.backend_service.getLatestElectricityMeasurements().subscribe(item => 
      {
        let index = 0;
        let raw = 0;

        item.forEach(element => {
          this.data.push({
            label: (index % 5 === 0) ? `${Math.round(index / 5)}` : '',
            value: element
          }),
          // round the number to 3 decimals
          raw = Math.round((element + Number.EPSILON) * 100) / 100
          this.totalPowerConsumption += raw;
          index++;
        });
        this.chartData = this.data;
        console.log(item);
        console.log(this.totalPowerConsumption);
      });
  }

  ngOnDestroy() {
    this.alive = false;
  }
}