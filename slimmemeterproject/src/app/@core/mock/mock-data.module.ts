import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ElectricityService } from './electricity.service';
import { TemperatureHumidityService } from './temperature-humidity.service';
import { SolarService } from './solar.service';

const SERVICES = [
  ElectricityService,
  TemperatureHumidityService,
  SolarService,
];

@NgModule({
  imports: [
    CommonModule,
  ],
  providers: [
    ...SERVICES,
  ],
})
export class MockDataModule {
  static forRoot(): ModuleWithProviders<MockDataModule> {
    return {
      ngModule: MockDataModule,
      providers: [
        ...SERVICES,
      ],
    };
  }
}
