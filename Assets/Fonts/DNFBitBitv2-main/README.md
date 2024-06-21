# 던파 비트비트체 v2

[배포처 바로가기](https://brand.nexon.com/ko/ci-brand-guidelines/typeface#section-bit-bit)

&nbsp;

## 웹 폰트

사용하는 `font-family`의 이름은 `DNFBitBitv2`입니다.

### HTML

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.css" type="text/css"/>
```

### CSS `@Import`

```css
@import url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.css');
```

### CSS `@font-face`

```css
@font-face {
    font-family: 'DNFBitBitv2';
    font-weight: normal;
    font-style: normal;
    font-display: swap;
    src: url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.woff2') format('woff2'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.woff') format('woff'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.otf') format('opentype'),
         url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/DNFBitBitv2.ttf') format('truetype');
}
```

&nbsp;

## 다이나믹 서브셋

웹폰트의 최적화를 위해 모던 브라우저에서는 글리프를 여러개로 나누어 필요한 부분만 동적으로 파싱하는 다이나믹 서브셋을 제공합니다. 폰트의 용량이 부담된다면 아래 코드를 사용하는 걸 추천합니다.

### HTML

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/subsets/DNFBitBitv2-dynamic-subset.css" type="text/css"/>
```

### CSS

```css
@import url('https://cdn.jsdelivr.net/gh/fonts-archive/DNFBitBitv2/subsets/DNFBitBitv2-dynamic-subset.css');
```

&nbsp;

## font-family

어느 브라우저나 시스템 환경에서도 동일한 폰트가 적용되어야 한다면 아래와 같이 구성하는 걸 추천합니다. `-apple-system`과 `BlinkMacSystemFont`는 맥, `Segoe UI`는 윈도우, `Roboto`는 안드로이드의 기본 폰트입니다.



```css
font-family: "DNFBitBitv2", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
```

&nbsp;

## 라이선스

라이선스는 언제든지 변경될 수 있습니다. 변경사항을 확인하려면 배포처를 방문해 주세요.

```
Q. 넥슨 서체의 저작권은 누구에게 있나요?
제공되는 모든 서체의 지적 재산권을 포함한 모든 권리는 넥슨코리아에 있습니다.

Q. 상업적인 용도로 넥슨 서체를 사용해도 되나요?
서체는 개인 및 기업 사용자를 포함한 모든 사용자에게 무료로 제공되며 자유롭게 사용 및 배포하실 수 있습니다. (상업적인 용도로 사용 가능) 단, 임의로 폰트 파일을 수정 편집하여 재배포할 수 없습니다.

Q. 넥슨 서체를 재판매 할 수 있나요?
글꼴 자체를 유료로 판매하는 것은 금지되며, 서체의 본 저작권 안내를 포함해서 다른 소프트웨어와 번들하거나 임베디드 폰트로 사용하실 수 있습니다.

Q. 넥슨 서체 사용시 출처를 꼭 밝혀야 하나요?
서체의 출처 표기를 권장합니다.

※ 서체를 사용한 인쇄물, 광고물(온라인포함)은 넥슨의 프로모션을 위해 활용될 수 있습니다.
```
