document.getElementById('showImg500').onclick = function () {
    document.getElementById('img500').hidden = false;
    document.getElementById('img300').hidden = true;
    document.getElementById('img800').hidden = true;
    document.getElementById('showImg800').className = 'btn btn-secondary';
    document.getElementById('showImg300').className = 'btn btn-secondary';
    document.getElementById('showImg500').className = 'btn btn-primary';
}

document.getElementById('showImg300').onclick = function () {
    document.getElementById('img500').hidden = true;
    document.getElementById('img300').hidden = false;
    document.getElementById('img800').hidden = true;
    document.getElementById('showImg800').className = 'btn btn-secondary';
    document.getElementById('showImg300').className = 'btn btn-primary';
    document.getElementById('showImg500').className = 'btn btn-secondary';
}

document.getElementById('showImg800').onclick = function () {
    document.getElementById('img500').hidden = true;
    document.getElementById('img300').hidden = true;
    document.getElementById('img800').hidden = false;
    document.getElementById('showImg800').className = 'btn btn-primary';
    document.getElementById('showImg300').className = 'btn btn-secondary';
    document.getElementById('showImg500').className = 'btn btn-secondary';
}