import { Component } from '@angular/core';
import { ChartConfiguration, ChartData, ChartEvent } from 'chart.js';
import { StatsService } from './stats.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  totalInstalls: number = 0;
  
  constructor(private statService: StatsService) {
    this.statService.getTotalUsers().subscribe(users => this.totalInstalls = users);
    
  }
}
