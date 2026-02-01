// QR Code Scanner using HTML5 QR Code library
window.qrScanner = {
    video: null,
    canvas: null,
    canvasContext: null,
    scanning: false,
    dotnetRef: null,

    async startScanner(videoElementId, dotnetReference) {
        this.dotnetRef = dotnetReference;
        this.video = document.getElementById(videoElementId);
        
        if (!this.video) {
            console.error('Video element not found');
            return false;
        }

        try {
            const stream = await navigator.mediaDevices.getUserMedia({
                video: { facingMode: 'environment' }
            });
            
            this.video.srcObject = stream;
            this.video.setAttribute('playsinline', true);
            this.video.play();
            
            this.scanning = true;
            requestAnimationFrame(() => this.tick());
            
            return true;
        } catch (err) {
            console.error('Error accessing camera:', err);
            alert('Nepodařilo se získat přístup ke kameře. Zkontrolujte oprávnění prohlížeče.');
            return false;
        }
    },

    tick() {
        if (!this.scanning || !this.video || this.video.readyState !== this.video.HAVE_ENOUGH_DATA) {
            if (this.scanning) {
                requestAnimationFrame(() => this.tick());
            }
            return;
        }

        if (!this.canvas) {
            this.canvas = document.createElement('canvas');
            this.canvas.width = this.video.videoWidth;
            this.canvas.height = this.video.videoHeight;
            this.canvasContext = this.canvas.getContext('2d', { willReadFrequently: true });
        }

        this.canvasContext.drawImage(this.video, 0, 0, this.canvas.width, this.canvas.height);
        const imageData = this.canvasContext.getImageData(0, 0, this.canvas.width, this.canvas.height);
        
        const code = jsQR(imageData.data, imageData.width, imageData.height, {
            inversionAttempts: 'dontInvert',
        });

        if (code && code.data) {
            this.onQRCodeDetected(code.data);
            return;
        }

        requestAnimationFrame(() => this.tick());
    },

    onQRCodeDetected(data) {
        console.log('QR Code detected:', data);
        if (this.dotnetRef) {
            this.dotnetRef.invokeMethodAsync('OnQRCodeScanned', data);
        }
    },

    stopScanner() {
        this.scanning = false;
        
        if (this.video && this.video.srcObject) {
            const tracks = this.video.srcObject.getTracks();
            tracks.forEach(track => track.stop());
            this.video.srcObject = null;
        }
        
        this.video = null;
        this.canvas = null;
        this.canvasContext = null;
        this.dotnetRef = null;
    }
};
