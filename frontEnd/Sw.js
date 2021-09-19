const staticCacheName = 'site-static-v7';
const dynamicCacheName = 'site-static-v7';

const assets = [
    '/LoginPage.html',
    '/JavaScript/app.js',
    '/JavaScript/userInterface/Ui.js',
    '/JavaScript/db.js',
    '/JavaScript/materialize.min.js',
    '/CSS/StyleSheet.css',
    '/CSS/materialize.min.css',
    '/Image/No_Foto.png',
    'https://fonts.googleapis.com/icon?family=Material+Icons',
    'https://fonts.gstatic.com/s/materialicons/v70/flUhRq6tzZclQEJ-Vdg-IuiaDsNc.woff2',
    '/HTML/fallback.html'
];


//cache size limit function
const limitCacheSize = (name, size) => {
    caches.open(name).then(cache => {
        cache.keys().then(keys => {
            if(keys.length > size){
                cache.delete(keys[0]).then(limitCacheSize(name, size));
            }
        })
    })
};

//Install Event
self.addEventListener('install', evt =>{
    console.log('Service worker has been installed');
    evt.waituntil(
        caches.open(staticCacheName).then(cache =>{ //Async task
            console.log('caching shell assets');
            cache.addAll(assets);
        })
    );
});

//Activate Event
self.addEventListener('activate', evt =>{
    console.log('service worker has been activated')
    evt.waituntil(
        caches.keys().then(keys =>{
            //console.log(keys);
            return Promise.all(keys
                .filter(key => key !== staticCacheName && key !== dynamicCacheName)
                .map(key => caches.delete(key))
            )
        })
    );
});

//fetch event
self.addEventListener('fetch', evt =>{
    console.log('fetch event', evt)
    evt.respondWith(
        caches.match(evt .request).then(cacheRes =>{
            return cacheRes || fetch(evt.request).then(fetchRes => {
                return caches.open(dynamicCacheName).then(cache =>{
                    cache.put(evt.request.url, fetchRes.clone());
                    limitCacheSize(dynamicCacheName, 15);
                    return fetchRes;
                })
            });
        }).catch(() => {
            if(evt.request.url.indexOf('.html') > -1){
                return caches.match('/HTML/fallback.html');
            }         
        })  
    );
});


function createDB(){
    indexedDB.open('products', 1, function(upgradeDB){
        var store = upgradeDB.createObjectStore('beverages', {
            keyPath: 'id'
        });
        
    })
}