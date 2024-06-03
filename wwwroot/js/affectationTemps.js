    var etapeId;

    function affecterEtape(id) {
        etapeId = id;
        getCoureursForEtape(id);
    }

    function getCoureursForEtape(etapeId) {
        $.ajax({
            url: '/Home/GetCoureursForEtape',
            type: 'GET',
            data: { etapeId: etapeId },
            success: function(coureurs) {
                var selectCoureurDropdown = document.getElementById("selectCoureur");
                selectCoureurDropdown.innerHTML = "";
                coureurs.forEach(function(coureur) {
                    var option = document.createElement("option");
                    option.value = coureur.idCoureur;
                    option.text = coureur.nom + " - " + coureur.equipe.nom;
                    selectCoureurDropdown.appendChild(option);
                });
            },
            error: function(xhr, status, error) {
                console.error("Une erreur s'est produite lors de la récupération des coureurs pour l'étape :", error);
            }
        });
    }

    function submitForm() {
        var selectedCoureur = document.getElementById("selectCoureur").value;
        var tempsLoc = document.getElementById("tempsLoc").value;
        console.log("bb " + tempsLoc);

        if (!etapeId) {
            alert("Veuillez sélectionner une étape avant de continuer.");
            return;
        }

        document.getElementById("etapeId").value = etapeId;
        document.getElementById("coureur").value = selectedCoureur;
        document.getElementById("temps").value = tempsLoc;
        document.getElementById("myForm").submit();
    }

    function finEtape(id) {
		var etape = id;
		console.log("etape: " + etape);

		if (!etape) {
			alert("Veuillez sélectionner une étape avant de continuer.");
			return;
		}

		$.ajax({
			url: '/Home/FinEtape',
			type: 'POST',
			data: { etapeFin: etape },
			success: function (response) {
                location.reload(true);
			},
			error: function (xhr, status, error) {
				console.error("Une erreur s'est produite :", error);
				alert("Une erreur s'est produite lors de la fin de l'étape.");
			}
		});
	}

