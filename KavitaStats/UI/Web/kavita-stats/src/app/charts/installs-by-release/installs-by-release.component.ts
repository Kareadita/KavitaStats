import { Component, OnInit } from '@angular/core';
import { ChartData, ChartConfiguration, ChartEvent } from 'chart.js';
import { StatsService } from 'src/app/stats.service';

@Component({
  selector: 'app-installs-by-release',
  templateUrl: './installs-by-release.component.html',
  styleUrls: ['./installs-by-release.component.scss']
})
export class InstallsByReleaseComponent implements OnInit {

  totalInstalls: number = 0;
  installsByRelease!: ChartData<'bar'>;
  barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: {
      x: {},
      y: {
        min: 1
      }
    },
    plugins: {
      legend: {
        display: true,
      }
    }
  };

  cutOffdays = 0;

  constructor(private statService: StatsService) { }

  ngOnInit(): void {
    this.statService.getInstallsByRelease().subscribe(versionCounts => {
      console.log(versionCounts);
      this.installsByRelease = {labels: [], datasets: []};
      this.installsByRelease.datasets.push({data: []});

      if (this.barChartOptions?.scales !== undefined && this.barChartOptions?.scales.y !== undefined) {
        this.barChartOptions.scales.y.max = Math.max(...versionCounts.map(r => r.installCount));
      }

      

      versionCounts.forEach(releaseInstallCount => {
        this.installsByRelease.labels?.push(releaseInstallCount.releaseVersion);
        this.installsByRelease.datasets[0].data.push(releaseInstallCount.installCount);
        this.totalInstalls += releaseInstallCount.installCount;
      });
    });
  }

  chartClicked({ event, active }: { event?: ChartEvent, active?: {}[] }): void {
    console.log('Clicked: ', event, active);
  }

  

}
