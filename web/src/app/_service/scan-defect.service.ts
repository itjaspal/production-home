import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { JobInProcessStockSearchView, JobInProcessStockView, ProductSubSearchView, SubProductView } from '../_model/job-inprocess-stock';
import { OrderReqSearchView, OrderReqView } from '../_model/job-operation-stock';
import { DataQcCuttingView } from '../_model/product-defect';
import { DefectProductSubSearchView, DefectProductSubView, ScanDefectSearchView, ScanDefectView } from '../_model/scan-defect';

@Injectable({
  providedIn: 'root'
})
export class ScanDefectService {

  constructor(
    private http: HttpClient,
  ) { }

  public async searchScanAdd(_model: ScanDefectSearchView) {
    return await this.http.post<ScanDefectView>(environment.API_URL + 'scan-defect/postScanAdd', _model).toPromise();
  }

  public async searchEntryAdd(_model: ScanDefectSearchView) {
    return await this.http.post<ScanDefectView>(environment.API_URL + 'scan-defect/postEntryAdd', _model).toPromise();
  }

  public async Cancel(params) {
    return await this.http.post<ScanDefectView>(environment.API_URL + 'scan-defect/postCancel', params).toPromise();
  }

  public async getSubProduct(_model: DefectProductSubSearchView) {
    return await this.http.post<DefectProductSubView[]>(environment.API_URL + 'scan-defect/postGetSubProduct', _model).toPromise();
  }

  public async getSubProductCancel(_model: DefectProductSubSearchView) {
    return await this.http.post<DefectProductSubView[]>(environment.API_URL + 'scan-defect/postGetSubProductCancel', _model).toPromise();
  }

  public async searchCancel(_model: ScanDefectSearchView) {
    return await this.http.post<ScanDefectView>(environment.API_URL + 'scan-defect/postEntryCancel', _model).toPromise();
  }

  public async EntryRemark(_model : DataQcCuttingView) {
    return await this.http.post<ScanDefectView>(environment.API_URL + 'scan-defect/postEntryRemark', _model).toPromise();
  }

  public async getSummaryDefect(_model: DefectProductSubSearchView) {
    return await this.http.post<DefectProductSubView[]>(environment.API_URL + 'scan-defect/postGetSummaryDefect', _model).toPromise();
  }

  public async getOrderReq(_model: OrderReqSearchView) {
    return await this.http.post<OrderReqView[]>(environment.API_URL + 'scan-defect/postGetOrderReq', _model).toPromise();
  }
}
