import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReleaseInstallCount } from './_models/release-install-count';
import { VolumesInASeries } from './_models/volumes-in-a-series';

@Injectable({
  providedIn: 'root'
})
export class StatsService {

  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getTotalUsers() {
    return this.httpClient.get<number>(this.baseUrl + 'ui/total-users', { responseType: 'text' as 'json' });
  }

  getInstallsByRelease(cutoffDays: number = 0) {
    return this.httpClient.get<Array<ReleaseInstallCount>>(this.baseUrl + 'ui/installs-by-release?cutoffDays=' + cutoffDays);
  }

  getVolumesInASeries() {
    return this.httpClient.get<VolumesInASeries>(this.baseUrl + 'ui/volumes-in-a-series');
  }
}
