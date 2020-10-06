import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { JobInProcessSearchView, JobInProcessView } from '../_model/job-inprocess';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class ScanInprocessService {

  constructor(
    private http: HttpClient
  ) { }

  public async searchscanadd(_model: JobInProcessSearchView) {
    return await this.http.post<JobInProcessView>(environment.API_URL + 'job-inproces/postSearchScanAdd', _model).toPromise();
  }
}
