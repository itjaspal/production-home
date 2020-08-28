import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: "arrayFilter"
})

export class BuilderFilterPipe implements PipeTransform {

  transform(items: any, filter: any, isAnd: boolean): any {
    if (filter && Array.isArray(items)) {
      let filterKeys = Object.keys(filter);
      if (isAnd) {
        return items.filter(item =>
          filterKeys.reduce((memo, keyName) =>
            (memo && new RegExp(filter[keyName].trim(), 'gi').test(item[keyName])) || filter[keyName].trim() === "", true));
      } else {
        return items.filter(item => {
          return filterKeys.some((keyName) => {

            return new RegExp(filter[keyName].trim(), 'gi').test(item[keyName]) || filter[keyName].trim() === "";
          });
        });
      }
    } else {
      return items;
    }
  }
}