using CustomerQueryWebAPI.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerQueryApp.Helpers
{
    public class CustomerWebAPI
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44382/");
            return Client;
        }


        public async Task<List<DeptViewModel>> PopulateDeptDropDown()
        {
            HttpClient client = Initial();
            HttpResponseMessage res = await client.GetAsync("api/department/getDepartments");
            List<DeptViewModel> pvmList = new List<DeptViewModel>();
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                pvmList = JsonConvert.DeserializeObject<List<DeptViewModel>>(result);
            }

            return pvmList;
        }

        public async Task<List<RoleViewModel>> PopulateRoleDropDown()
        {
            HttpClient client = Initial();
            HttpResponseMessage res = await client.GetAsync("api/role/getRoles");
            List<RoleViewModel> pvmList = new List<RoleViewModel>();
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                pvmList = JsonConvert.DeserializeObject<List<RoleViewModel>>(result);
            }

            return pvmList;
        }

    }
}
