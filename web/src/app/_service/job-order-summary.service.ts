import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { environment } from '../../environments/environment';
import { OrderSummaryProdDetailView, OrderSummaryProductDetailView, OrderSummarySearchView, SpecDrawingView } from '../_model/job-operation';

@Injectable({
  providedIn: 'root'
})
export class JobOrderSummaryService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,

  ) { 
    
   // this.user = this._authSvc.getLoginUser();

  }

  public async searchOrderSummary(_model: OrderSummarySearchView) { 

    console.log('searchOrderSummary _model.pageIndex: '+ _model.pageIndex);
    console.log('searchOrderSummary _model.itemPerPage: '+ _model.itemPerPage);
    console.log('searchOrderSummary _model.build_type: '+ _model.build_type);
    console.log('searchOrderSummary _model.pdjit_grp : '+ _model.pdjit_grp);
    console.log('searchOrderSummary _model.req_date : '+ _model.req_date); 
   

    return await this.http.post<OrderSummaryProdDetailView<OrderSummaryProductDetailView>>(environment.API_URL + 'job-operation/orderSummary', _model).toPromise();  

  } 

  public async getSpecDrawing(_pBarcode: string , _pDsgn_no ) { 

    return await this.http.get<SpecDrawingView>(environment.API_URL + 'spec/getdrawning/' + _pBarcode+'/'+_pDsgn_no).toPromise();

  }

  public async getSpecDrawingBarcode(_pBarcode: string ) { 

    return await this.http.get<SpecDrawingView>(environment.API_URL + 'spec/getdrawningall/' + _pBarcode).toPromise();

  }



}
