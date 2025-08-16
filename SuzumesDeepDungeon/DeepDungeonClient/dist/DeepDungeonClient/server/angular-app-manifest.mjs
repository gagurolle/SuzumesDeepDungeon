
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/login"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 24610, hash: 'af735699118ba4ed2a6393aa8df0ba1165c0168aa0ca366489a99bdb0719feff', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 17063, hash: '7973b2901d21b5e1eb9dbfdbc6484ecf06c5106abae19030248b706bc19f8eb5', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 36525, hash: '5a20a0309437ab6ea57e52661ad5e0b48b4c4412106995e6b9d205e818e593e6', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 36525, hash: '5a20a0309437ab6ea57e52661ad5e0b48b4c4412106995e6b9d205e818e593e6', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'styles-DTTV3AOM.css': {size: 8100, hash: 'jHWbwFO0LXY', text: () => import('./assets-chunks/styles-DTTV3AOM_css.mjs').then(m => m.default)}
  },
};
