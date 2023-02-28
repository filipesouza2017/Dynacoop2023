if (typeof (AlfaPeople) == "undefined") { AlfaPeople = {} }
if (typeof (AlfaPeople.Account) == "undefined") { AlfaPeople.Account = {} }

AlfaPeople.Account = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();
                
    },
    CNPJOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cnpj = formContext.getAttribute("dcp_cnpj").getValue();

        if (cnpj != null) {
            if (cnpj.length == 14) {
                var formattedCNPJ = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
                var id = Xrm.Page.data.entity.getId();

                var queryAccountId = "";

                if (id.length > 0) {
                    queryAccountId += " and accountid ne " + id;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("account", "?$select=name&$filter=dcp_cnpj eq '" + formattedCNPJ + "'" + queryAccountId).then(
                    function success(results) {
                        if (results.entities.length == 0) {
                            formContext.getAttribute("dcp_cnpj").setValue(formattedCNPJ);
                        } else {
                            formContext.getAttribute("dcp_cnpj").setValue("");
                            AlfaPeople.Account.DynamicsAlert("O CNPJ já existe no sistema", "CNPJ duplicado!")
                        }
                    },
                    function (error) {
                        AlfaPeople.Account.DynamicsAlert("Por favor contato o administrador", "Erro do sistema")
                    }
                );
            }
            else {
                AlfaPeople.Account.DynamicsAlert("O CNPJ digitado não é valido", "CNPJ inválido!")
            }
        } else {
            AlfaPeople.Account.DynamicsAlert("Informe um valor para o CNPJ", "CNPJ incorreto!")
        }
    },
    DynamicsAlert: function (alertText, alertTitle) {
        var alertStrings = {
            confirmButtonLabel: "OK",
            text: alertText,
            title: alertTitle
        };

        var alertOptions = {
            height: 120,
            width: 200
        };

        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions);
    },
    FormControls: function () {
        formContext.getControl("industrycode").setDisabled(true);

        //SetRequiredLevel
        formContext.getAttribute("primarycontactid").setRequiredLevel("required");

        //SetVisible
        formContext.getControl("fax").setVisible(false);
    },
    GetSet: function () {
        //Get
        //Número Inteiro
        var numeroTotalOpp = formContext.getAttribute("dcp_nmr_total_opp").getValue();

        //Texto
        var nomeDaConta = formContext.getAttribute("name").getValue();

        //Picklist
        //Seguros == 20
        var setor = formContext.getAttribute("industrycode").getValue();

        //Money ou decimal
        var valorTotalDeOpp = formContext.getAttribute("dcp_valor_total_opp").getValue();

        //Lookup ou Consulta
        var lookup = formContext.getAttribute("primarycontactid").getValue();

        //Set
        //Número Inteiro
        formContext.getAttribute("dcp_nmr_total_opp").setValue(10);

        //Text
        formContext.getAttribute("name").setValue("Novo nome da Conta!");

        //Picklist
        formContext.getAttribute("industrycode").setValue(20);

        //Money ou decimal
        formContext.getAttribute("dcp_valor_total_opp").setValue(10.50);

        //Lookup ou Consulta
        var lookup = [];
        lookup[0] = {};
        lookup[0].name = "Gonçalves, Diego";
        lookup[0].id = "cdd6a450-cb0c-ea11-a813-000d3a1b1223";
        lookup[0].entityType = "contact";
        formContext.getAttribute("primarycontactid").setValue(lookup);
    }
}