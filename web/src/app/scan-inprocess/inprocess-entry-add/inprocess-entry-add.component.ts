import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild  } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { JobInProcessView, JobInProcessSearchView, JobInProcessScanFinView, JobInProcessScanView } from '../../_model/job-inprocess';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';
import { DatePipe } from '@angular/common';
import { ProductSearchComponent } from '../product-search/product-search.component';



@Component({
  selector: 'app-inprocess-entry-add',
  templateUrl: './inprocess-entry-add.component.html',
  styleUrls: ['./inprocess-entry-add.component.scss']
})
export class InprocessEntryAddComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobInprocessSvc: ScanInprocessService,
    private cdr: ChangeDetectorRef
    
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: JobInProcessView = new JobInProcessView();
  public searchModel: JobInProcessSearchView = new JobInProcessSearchView();

  public data: any = {};
  public model_scan: JobInProcessScanFinView = new JobInProcessScanFinView();

  public datas: any = {};
  public count = 0;

  public result: any = {};

  
  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      bar_code: [null, [Validators.required]],
      qty: [null, []]
    });
  }


  async save() {

    var datePipe = new DatePipe("en-US");
    this.searchModel.wc_code = this._actRoute.snapshot.params.wc_code;
    this.searchModel.req_date = datePipe.transform(this._actRoute.snapshot.params.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.searchModel.pdjit_grp = this._actRoute.snapshot.params.pdjit_grp;
    //console.log(this.searchModel);
    
    this.datas = await this._jobInprocessSvc.searchentryadd(this.searchModel);

    this.add(this.datas);
    this.searchModel.bar_code = "";
    this.searchModel.qty = 0;

  }

  add(datas: any) {

    let newProd: JobInProcessScanView = new JobInProcessScanView();
    newProd.prod_code = datas.prod_code;
    newProd.prod_name = datas.prod_name;
    newProd.qty = datas.qty;


    this.model_scan.datas.push(newProd);

    // Group By Product
    var result = [];
    this.model_scan.datas.forEach(function (a) {
      if (!this[a.prod_code] && !this[a.prod_name]) {
        this[a.prod_code] = { prod_code: a.prod_code, prod_name: a.prod_name, qty: 0 };
        result.push(this[a.prod_code]);
      }
      this[a.prod_code].qty += a.qty;

    }, Object.create(null));

    this.model_scan.datas = result;
    //console.log(result)
    console.log(this.model_scan.datas);


  }


  openSearchProductModal(SaleTransactionItemView = null)
  {
    const dialogRef = this._dialog.open(ProductSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        req_date: this._actRoute.snapshot.params.req_date,
        wc_code:this._actRoute.snapshot.params.wc_code,
        pdjit_grp:this._actRoute.snapshot.params.pdjit_grp
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_prod(result);
      }
    })
  }

  add_prod(datas: any) {

    console.log(datas);
    this.searchModel.bar_code = datas[0].bar_code;
    this.searchModel.qty = 1;
  }  


  close() {
    this._router.navigateByUrl('/app/scaninproc/inprocserach/' + this._actRoute.snapshot.params.req_date + '/' + this._actRoute.snapshot.params.wc_code);
  }


}
