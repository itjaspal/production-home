import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { JobInProcessStockSearchView, JobInProcessStockView, ProductSubSearchView, ScanInprocessSearchView, SubProductView } from '../_model/job-inprocess-stock';
import { JobOperationStockSearchView, OrderReqSearchView, OrderReqView } from '../_model/job-operation-stock';
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

  public async searchInProcessDefect(_model: JobOperationStockSearchView) { 
    return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-inprocess-stock/postSearchJobInPorcessDefect', _model).toPromise();   
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

  public async getSubProduct(_model: ProductSubSearchView) {
    return await this.http.post<SubProductView[]>(environment.API_URL + 'job-inprocess-stock/postGetSubProduct', _model).toPromise();
  }

  public async getSubProductCancel(_model: ProductSubSearchView) {
    return await this.http.post<SubProductView[]>(environment.API_URL + 'job-inprocess-stock/postGetSubProductCancel', _model).toPromise();
  }

  public async searchCancel(_model: JobInProcessStockSearchView) {
    return await this.http.post<JobInProcessStockView>(environment.API_URL + 'job-inprocess-stock/postEntryCancel', _model).toPromise();
  }

  public async getOrderReq(_model: OrderReqSearchView) {
    return await this.http.post<OrderReqView[]>(environment.API_URL + 'job-inprocess-stock/postGetOrderReq', _model).toPromise();
  }

}
