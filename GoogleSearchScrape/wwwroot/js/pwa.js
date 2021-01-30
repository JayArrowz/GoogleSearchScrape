var layoutRef = null;
const blazorInstallMethod = 'ShowInstallPrompt';

export function setLayoutRef(ref) {
    layoutRef = ref;
}
export function showAddToHomeScreen() {
    if (layoutRef == null) {
        setTimeout(showAddToHomeScreen, 1000);
    } else {
        layoutRef.invokeMethodAsync(blazorInstallMethod)
            .then(function () { layoutRef = null; }, function (er) { setTimeout(showAddToHomeScreen, 1000); });
    }
}

export function installPWA() {
    var prompt = window.PWADeferredPrompt;
    if (prompt) {
        prompt.prompt();
        prompt.userChoice
            .then(function (choiceResult) {
                window.PWADeferredPrompt = null;
            });
    }
}

showAddToHomeScreen();