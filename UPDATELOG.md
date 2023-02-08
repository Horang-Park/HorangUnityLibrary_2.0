### 2.0.1_20230208_5340fc8c84927e796d2b5e5d351e9f1426107f98
    1. 빌드할 때, Editor 코드가 포함되어 에러가 발생하는 현상 수정
    2. 자주 사용할 가능성이나 단일 기능만을 수행하는 기능들은 struct로 전환
    3. 오디오 모듈 에셋 생성 방식을 상단 메뉴에 있는 메뉴로 진입하여 생성하도록 수정
    4. 외부에서 라이브러리 내부에 있는 사용 안해도 되는 코드를 노출시키지 않게하기 위하여 접근제한자 수정
    5. PlayerPrefs를 래핑하는 기능 추가
        5-1. AES 256으로 키와 값을 암호화
        5-2. int, IEnumerable<int>, long, IEnumerable<long>, float, IEnumerable<float>,
             double, IEnumerable<double>, string, IEnumerable<string> 지원
    6. CSV 파싱 기능 추가

### 2.0.0_20230207_832978baa87fef8183afdac5a66af29ec93c8044
    1. HorangUnityLibrary 2.0 첫 릴리즈