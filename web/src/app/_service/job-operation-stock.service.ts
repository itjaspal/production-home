import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ProductionTrackingSearchView, ProductionTrackingView } from '../_model/job-operation';
import { JobOperationStockSearchView, OrderReqSearchView, OrderReqView, ProductGroupSearchView, ProductionTrackingStockSearchView} from '../_model/job-operation-stock';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class JobOperationStockService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,
  ) { }

  public async searchDataPlan(_model: JobOperationStockSearchView) { 
     return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-operation-stock/postSearchDataPlan', _model).toPromise();   
   } 

   public async searchDataFin(_model: JobOperationStockSearchView) { 
    return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-operation-stock/postSearchDataFin', _model).toPromise();   
  } 

  public async searchDataDefect(_model: JobOperationStockSearchView) { 
    return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-operation-stock/postSearchDataDefect', _model).toPromise();   
  } 

  public async SearchSummaryProdcutGroup(_model: ProductGroupSearchView) { 
    return await this.http.post<ProductGroupSearchView>(environment.API_URL + 'job-operation-stock/postSearchSummaryProdcutGroup', _model).toPromise();   
  } 

  public async searchProductionTrackingStock(_model: ProductionTrackingStockSearchView) {
    return await this.http.post<ProductionTrackingView>(environment.API_URL + 'job-operation-stock/postProductionTrackingStock', _model).toPromise();
  }

  public async searchProductionTrackingDetailStock(_model: ProductionTrackingStockSearchView) {
    return await this.http.post<ProductionTrackingView>(environment.API_URL + 'job-operation-stock/postProductionTrackingDetailStock', _model).toPromise();
  }

  public async getOrderReqAll(_model: OrderReqSearchView) {
    return await this.http.post<OrderReqView[]>(environment.API_URL + 'job-operation-stock/postGetOrderReqAll', _model).toPromise();
  }
}
