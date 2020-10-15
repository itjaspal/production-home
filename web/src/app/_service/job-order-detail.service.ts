import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { environment } from '../../environments/environment';
import { OrderInfoHeaderView } from '../_model/job-operation';

@Injectable({
  providedIn: 'root'
})
export class JobOrderDetailService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,

  ) { 
    
   // this.user = this._authSvc.getLoginUser();

  }

  public async getOrderInfoDetail(_pEntity: string, _pPorNo: string) { 

    /*console.log('searchOrderSummary _model.pageIndex: '+ _model.pageIndex);
    console.log('searchOrderSummary _model.itemPerPage: '+ _model.itemPerPage);
    console.log('searchOrderSummary _model.build_type: '+ _model.build_type);
    console.log('searchOrderSummary _model.pdjit_grp : '+ _model.pdjit_grp);
    console.log('searchOrderSummary _model.req_date : '+ _model.req_date); */
   

    //return await this.http.post<OrderSummaryProdDetailView<OrderSummaryProductDetailView>>(environment.API_URL + 'job-operation/orderSummary', _model).toPromise();  
    return await this.http.get<OrderInfoHeaderView>(environment.API_URL + 'job-operation/getOrderInfo/' + _pEntity + '/' + _pPorNo).toPromise();

  } 
} 