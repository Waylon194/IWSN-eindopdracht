import { Component, OnDestroy, OnInit } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { TemperatureHumidityData } from '../../../@core/data/temperature-humidity';
import { takeWhile } from 'rxjs/operators';
import { forkJoin, Observable } from 'rxjs';
import { IwsnBackendService } from '../../../iwsn-backend/iwsn-backend.service'; 

@Component({
  selector: 'ngx-temperature',
  styleUrls: ['./temperature.component.scss'],
  templateUrl: './temperature.component.html',
})
export class TemperatureComponent implements OnDestroy, OnInit {
  private alive = true;
  temperature: Number;
  theme: any;

  constructor(private themeService: NbThemeService,
    public iwsnBackendService: IwsnBackendService) {
    this.themeService.getJsTheme()
      .pipe(takeWhile(() => this.alive))
      .subscribe(config => {
      this.theme = config.variables.temperature;
    });
  }

  ngOnInit(): void {    
    this.iwsnBackendService.getLatestMeasurement().subscribe(item => {     
      this.temperature = <Number> item.datagram.telegram.instantaneousElectricityUsage;      
      console.log(item);
    })
  }

  ngOnDestroy() {
    this.alive = false;
  }
}