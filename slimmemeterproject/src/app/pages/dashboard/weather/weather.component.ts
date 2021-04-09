import { Component } from '@angular/core';
import { DatePipe } from '@angular/common';
import { IwsnBackendService } from '../../../iwsn-backend/iwsn-backend.service'; 

@Component({
  selector: 'ngx-weather',
  styleUrls: ['./weather.component.scss'],
  templateUrl: './weather.component.html',
  providers: [DatePipe]
})

export class WeatherComponent {
  temperature: Number;

  todayDate = new Date();
  myDate: string = ''; 
  constructor(private datePipe: DatePipe, private backend_service: IwsnBackendService){
      this.myDate = this.datePipe.transform(this.todayDate, 'yyyy-MM-dd');
  }

  ngOnInit(): void {    
    this.backend_service.getLatestTemp().subscribe(item => {     
      this.temperature = <Number> item.datagram.telegram.instantaneousElectricityUsage;      
      console.log(item);
    })
  }
}