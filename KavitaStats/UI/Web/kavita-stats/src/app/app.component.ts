import { Component } from '@angular/core';
import { ChartConfiguration, ChartData } from 'chart.js';
import { StatsService } from './stats.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  totalUsers: number = 0;
  installsByRelease!: ChartData<'bar'>;
  
  constructor(public statService: StatsService) {
    this.statService.getTotalUsers().subscribe(users => this.totalUsers = users);
    this.statService.getInstallsByRelease().subscribe(versionCounts => {
      console.log(versionCounts);
      this.installsByRelease.labels = versionCounts.map(r => r.releaseVersion);
      this.installsByRelease.datasets.push({data: []});

      versionCounts.forEach(releaseInstallCount => {
        this.installsByRelease.labels?.push(releaseInstallCount.releaseVersion);
        this.installsByRelease.datasets[0].data[0] = releaseInstallCount.installCount;
      });
    });
  }
}
