import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanReceiveCreateComponent } from '../../scan-receive/scan-receive-create/scan-receive-create.component';
import { ScanReceiveProductViewComponent } from '../../scan-receive/scan-receive-product-view/scan-receive-product-view.component';
import { ProductDefectView, ProductDefectSearchView } from '../../_model/product-defect';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ProductDefectService } from '../../_service/product-defect.service';
import { OrderDetailComponent } from '../order-detail/order-detail.component';
import { ProductDefectQcCuttingComponent } from '../product-defect-qc-cutting/product-defect-qc-cutting.component';
import { ProductDefectQcEntryComponent } from '../product-defect-qc-entry/product-defect-qc-entry.component';
import { ViewSpecComponent } from '../view-spec/view-spec.component';

@Component({
  selector: 'app-product-defect-search',
  templateUrl: './product-defect-search.component.html',
  styleUrls: ['./product-defect-search.component.scss']
})
export class ProductDefectSearchComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _defectSvc: ProductDefectService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ProductDefectView = new ProductDefectView();
  public searchModel: ProductDefectSearchView = new ProductDefectSearchView();

  public data: any = {};
  public datas: any = {};
  public datas_print: any = {};
  public data_qty : any;
  public data_reqdate : any;
  public reqDate : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.reqDate = new Date();
    this.searchModel.req_date = this.reqDate;
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      por_no: [null, []],
      req_date: [null, [Validators.required]],
    });
  }
  
  async search(event: PageEvent = null) {   
    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data_reqdate = this.searchModel.req_date;
    
     
    var datePipe = new DatePipe("en-US");
    this.searchModel.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;

    console.log(this.searchModel);
    this.data =  await this._defectSvc.searchDataProductDefect(this.searchModel); 
    console.log(this.data);
   
    // this.fin_date.nativeElement.value = this.data_findate;
    this.searchModel.req_date = this.data_reqdate;
   
}


openCuttingQC(p_por_no : string ,p_ref_no: string ,p_prod_code:string , p_size_name : string , p_qc_qty : number , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ProductDefectQcCuttingComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        entity:this.searchModel.entity,
        por_no: p_por_no,
        ref_no : p_ref_no,
        prod_code : p_prod_code,
        size_name : p_size_name,
        qc_qty : p_qc_qty
      }

    });

 
  }

  openEntryQC(p_por_no : string ,p_ref_no: string ,p_prod_code  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ProductDefectQcEntryComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        entity:this.searchModel.entity,
        por_no: p_por_no,
        ref_no : p_ref_no,
        prod_code : p_prod_code,
        build_type : this.user.branch.entity_code
      }

    });
    dialogRef.afterClosed().subscribe(result => {
      // if (result.length > 0) {
        this.search();
      // }
      
    })
    
  }

  openOrderDetail(p_por_no : string  , _index: number = -1)
  {
    console.log(this.searchModel.entity);
    const dialogRef = this._dialog.open(OrderDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        entity_code:this.searchModel.entity,
        por_no: p_por_no,
      }

    });

    
  }

  openSpec(p_bar_code : string  , _index: number = -1)
  {
    console.log(p_bar_code);
    const dialogRef = this._dialog.open(ViewSpecComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {    
        bar_code: p_bar_code,
      }

    });

    
  }

  close() {
    this._router.navigateByUrl('/app/home');
  }

}
