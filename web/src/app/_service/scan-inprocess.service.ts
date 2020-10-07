import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Dropdownlist } from '../_model/dropdownlist';
import { JobInProcessSearchView, JobInProcessView, ProductSearchView } from '../_model/job-inprocess';
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

  public async searchscancancel(_model: JobInProcessSearchView) {
    return await this.http.post<JobInProcessView>(environment.API_URL + 'job-inproces/postSearchScanCancel', _model).toPromise();
  }

  public async searchentryadd(_model: JobInProcessSearchView) {
    return await this.http.post<JobInProcessView>(environment.API_URL + 'job-inproces/postSearchEntryAdd', _model).toPromise();
  }

  public async searchentrycancel(_model: JobInProcessSearchView) {
    return await this.http.post<JobInProcessView>(environment.API_URL + 'job-inproces/postSearchEntryCancel', _model).toPromise();
  }

  public async getproduct(_model: ProductSearchView) {
    return await this.http.post<Dropdownlist[]>(environment.API_URL + 'job-inproces/getProduct', _model).toPromise();
  }
}
