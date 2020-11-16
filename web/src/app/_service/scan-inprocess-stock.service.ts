import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { JobInProcessStockSearchView, JobInProcessStockView } from '../_model/job-inprocess-stock';
import { JobOperationStockSearchView } from '../_model/job-operation-stock';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class ScanInprocessStockService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,
  ) { }

  public async searchInProcessPlan(_model: JobOperationStockSearchView) { 
     return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-inprocess-stock/postSearchJobInPorcessPlan', _model).toPromise();   
   } 

   public async searchInProcessFin(_model: JobOperationStockSearchView) { 
    return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-inprocess-stock/postSearchJobInPorcessFin', _model).toPromise();   
  } 

  public async searchScanAdd(_model: JobInProcessStockSearchView) {
    return await this.http.post<JobInProcessStockView>(environment.API_URL + 'job-inprocess-stock/postScanAdd', _model).toPromise();
  }

  public async searchEntryAdd(_model: JobInProcessStockSearchView) {
    return await this.http.post<JobInProcessStockView>(environment.API_URL + 'job-inprocess-stock/postEntryAdd', _model).toPromise();
  }

  public async Cancel(params) {
    return await this.http.post<JobInProcessStockView>(environment.API_URL + 'job-inprocess-stock/postCancel', params).toPromise();
  }
}
