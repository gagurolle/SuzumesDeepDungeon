
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
  },
  {
    "renderMode": 2,
    "route": "/profile"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 24610, hash: '1b1e69598bba66493ca6400c52ce20dc63db507a2028821c775314ecd50ea8e6', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 17063, hash: '9e274cf98c5bb0b5d766271bfed791fc9427f25da66451a2cdb44de39de2e27e', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'login/index.html': {size: 67646, hash: '03f0ac73afb283ddd6fe8c130307cb6415824c2c38261899456a6474ab2bc0ed', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'profile/index.html': {size: 67646, hash: '03f0ac73afb283ddd6fe8c130307cb6415824c2c38261899456a6474ab2bc0ed', text: () => import('./assets-chunks/profile_index_html.mjs').then(m => m.default)},
    'index.html': {size: 97409, hash: '3fdab35da165328d3a568f96a1b9f5216d54dc82c3d6b68719d4137680c294ef', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'styles-M5APJLGZ.css': {size: 8815, hash: 'sqgcnVJUoAI', text: () => import('./assets-chunks/styles-M5APJLGZ_css.mjs').then(m => m.default)}
  },
};
