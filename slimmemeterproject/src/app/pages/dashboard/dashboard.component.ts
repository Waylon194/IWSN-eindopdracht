import {Component, OnDestroy} from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { takeWhile } from 'rxjs/operators' ;
import { SolarData } from '../../@core/data/solar';

@Component({
  selector: 'ngx-dashboard',
  styleUrls: ['./dashboard.component.scss'],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnDestroy {
  private alive = true;
  solarValue: number;

  constructor(private themeService: NbThemeService,
    private solarService: SolarData) {
    this.themeService.getJsTheme()
    .pipe(takeWhile(() => this.alive))
    .subscribe()

    this.solarService.getSolarData()
    .pipe(takeWhile(() => this.alive))
    .subscribe((data) => {
    this.solarValue = data;
    });
  }

  ngOnDestroy() {
    this.alive = false;
  }
}
